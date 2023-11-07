﻿using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;
using RunGroops.ViewModels;

namespace RunGroops.Controllers {
    public class ClubController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(ApplicationDbContext context, IClubRepository clubRepository, IPhotoService photoService) {
            this._context = context;
            this._clubRepository = clubRepository;
            this._photoService = photoService;
        }
        public async Task<IActionResult> Index() {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id) {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubViewModel) {
            if (ModelState.IsValid) {
                var result = await _photoService.AddPhotoAsync(clubViewModel.Image);
                var club = new Club {
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Image = result.Url.ToString(),
                    Address = new Address {
                        Street = clubViewModel.Address.Street,
                        City = clubViewModel.Address.City,
                        State = clubViewModel.Address.State
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubViewModel);
        }
        public async Task<IActionResult> Edit(int id) {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null) {
                return View("Error");
            }
            var clubViewModel = new EditClubViewModel {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = new Address {
                    Street = club.Address.Street,
                    City = club.Address.City,
                    State = club.Address.State
                },
                ClubCategory = club.ClubCategory,
                URL = club.Image
            };
            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel editClubViewModel) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Failed to edit club");
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View("Edit", editClubViewModel);

            }
            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub != null) {
                try {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex) {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View("Edit", editClubViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(editClubViewModel.Image);
                var club = new Club {
                    Id = id,
                    Title = editClubViewModel.Title,
                    Description = editClubViewModel.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = editClubViewModel.AddressId,
                    Address = new Address {
                        Street = editClubViewModel.Address.Street,
                        City = editClubViewModel.Address.City,
                        State = editClubViewModel.Address.State
                    }
                };
                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else {
                return View(editClubViewModel);
            }
        }
    }
}
