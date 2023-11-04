﻿using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;

namespace RunGroops.Controllers {
    public class ClubController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;

        public ClubController(ApplicationDbContext context, IClubRepository clubRepository) {
            this._context = context;
            this._clubRepository = clubRepository;
        }
        public async Task<IActionResult> Index() {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id) {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }
    }
}