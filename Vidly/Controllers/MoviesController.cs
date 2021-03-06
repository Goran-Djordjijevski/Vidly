﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Movies
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanManageMovies))
            {
                return View("List");
            }

            return View("ReadOnlyList");
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(statusCode: HttpStatusCode.BadRequest);
            }

            var movie = _context.Movies.Include(c => c.Genre).SingleOrDefault(c => c.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);

            return View(movie);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit([Bind(Include = "Id, Name, ReleaseDate, NumberInStocks, GenreId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(movie).State = EntityState.Modified;
                movie.DateAdded = DateTime.Now;
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageMovies)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Name, ReleaseDate, NumberInStocks, GenreId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }
    }
}