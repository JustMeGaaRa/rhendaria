﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction;
using Rhendaria.Web.Extensions;
using Rhendaria.Web.Models;

namespace Rhendaria.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IClusterClient _client;

        public GameController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return RedirectToAction(nameof(HomeController.Error), this.NameOf<HomeController>());
            }

            Vector2D position = await _client.GetGrain<IPlayerActor>(username).GetPosition();

            PlayerStateViewModel viewModel = new PlayerStateViewModel
            {
                Username = username,
                PlayerPositionX = position.Left,
                PlayerPositionY = position.Top
            };

            return View(viewModel);
        }
    }
}