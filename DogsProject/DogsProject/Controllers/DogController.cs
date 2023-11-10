﻿using System.Net;

using DogsApp.Core.Contracts;
using DogsApp.Infrastructure.Data;
using DogsApp.Infrastructure.Data.Domain;

using DogsProject.Models.Dog;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.FileProviders;

namespace DogsProject.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        public DogController(IDogService dogService)
        { 
        this._dogService= dogService;
        }

        // GET: DogController/Details/5
        public IActionResult Details(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item==null)
            {
                return NotFound();
            }
            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // GET: DogController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DogCreateViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var created = _dogService.Create(bindingModel.Name, bindingModel.Age, bindingModel.Breed, bindingModel.Picture);
                if (created)
                {
                    return this.RedirectToAction("Success");
                }
            }
            return this.View();
        }
        public IActionResult Success()
        {
            return this.View();
        }
        public IActionResult Index(string searchStringBreed, string searchStringName)
        {
            List<DogAllViewModel> dogs = _dogService.GetDogs(searchStringBreed,searchStringName)
                    .Select(dogFromDb => new DogAllViewModel
                    {
                        Id = dogFromDb.Id,
                        Name = dogFromDb.Name,
                        Age = dogFromDb.Age,
                        Breed = dogFromDb.Breed,
                        Picture = dogFromDb.Picture
                    }).ToList();
            return View(dogs);
        }
        // GET: DogController/Edit/5
        public IActionResult Edit(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item==null)
            {
                return NotFound();
            }
            DogEditViewModel dog = new DogEditViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DogEditViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var updated = _dogService.UpdateDog(id, bindingModel.Name, bindingModel.Age, bindingModel.Breed, bindingModel.Picture);
                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(bindingModel);
        }

        // GET: DogController/Delete/5
        public IActionResult Delete(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item==null)
            {
                return NotFound();
            }
            DogEditViewModel dog = new DogEditViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _dogService.RemoveById(id);
            if (deleted)
            {
                return this.RedirectToAction("Index", "Dog");
            }
            else { return View(); }
        }
    }
}
