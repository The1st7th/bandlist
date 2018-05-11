using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BandList.Models;
using System;
namespace BandList.Controllers
{
    public class BandsController : Controller
    {
        [HttpGet("/bands")]
        public ActionResult Bands()
        {
            List<Band> allBands = Band.GetAll();
            return View(allBands);
        }

        [HttpGet("/band/new")]
        public ActionResult CreateBand()
        {
            List<Venue> allBands = Venue.GetAll();
            return View(allBands);
        }

        [HttpPost("/band")]
        public ActionResult Create()
        {
          Band newItem = new Band (Request.Form["new-band"]);
          newItem.Save();
          Venue selectedVenue = Venue.Find(int.Parse(Request.Form["venue"]));
          newItem.AddVenue(selectedVenue);
          List<Band> allBands = Band.GetAll();
          return View("Bands", allBands);
        }
        [HttpGet("/")]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost("/venue")]
        public ActionResult Indexv()
        {
            Venue newVenue = new Venue (Request.Form["venue"]);
            newVenue.Save();
            List<Venue> allVenues = Venue.GetAll();
            return View(allVenues);
        }
        [HttpGet("/venue/new")]
        public ActionResult Createvenue()
        {
            return View();
        }
        [HttpGet("/band/delete/{id}")]
        public ActionResult Delete(int id)
        {
            Band.Delete(id);
            List<Band> allBands = Band.GetAll();
            return View("Index", allBands);
        }
        [HttpPost("/bands/delete")]
        public ActionResult DeleteAll(int id)
        {
            Band.DeleteAll();
            List<Band> allBands =  Band.GetAll();
            return View("bands", allBands);
        }
        [HttpPost("/venue/search")]
        public ActionResult Resultcategory()
        {
          Venue selectedvenue = Venue.Find(int.Parse(Request.Form["venue"]));
          return View("bands", selectedvenue.GetBands());
        }
        [HttpGet("/venue/search")]
        public ActionResult Searchvenue()
        {
          List<Venue> allVenues = Venue.GetAll();
          return View(allVenues);
        }
        [HttpGet("/venues")]
        public ActionResult indexv()
        {
            List<Venue> allVenues = Venue.GetAll();
            return View(allVenues);
        }
        [HttpPost("/band/search")]
        public ActionResult Resultitems()
        {
          Band selectedbands = Band.Find(int.Parse(Request.Form["band"]));
          return View("Indexv", selectedbands.GetVenues());
        }
        [HttpGet("/band/search")]
        public ActionResult Searchvenuesfrombands()
        {
          List<Band> allItems = Band.GetAll();
          return View(allItems);
        }
        [HttpGet("/update")]
        public ActionResult update()
        {
          List<Band> allBands = Band.GetAll();
          return View(allBands);
        }
        [HttpPost("/update")]
        public ActionResult updateresult()
        {
          string valuename = Request.Form["value"];
          Console.WriteLine(valuename);
          Band.Find(int.Parse(Request.Form["band"])).UpdateDescription(valuename);
          List<Band> allItems = Band.GetAll();
          return View("bands",allItems);
        }
        [HttpGet("/add")]
        public ActionResult Add()
        {
          Dictionary<string, object> allgroup = new Dictionary<string, object>();
          List<Band> allBands = Band.GetAll();
          List<Venue> allVenues = Venue.GetAll();
          allgroup.Add("band", allBands);
          allgroup.Add("venue", allVenues);

          return View(allgroup);
        }
        [HttpPost("/add")]
        public ActionResult Added()
        {
          Band newItem = Band.Find(int.Parse(Request.Form["band"]));
          Venue selectedVenue = Venue.Find(int.Parse(Request.Form["venue"]));
          newItem.AddVenue(selectedVenue);
          List<Band> allBands = Band.GetAll();
          return View("Bands", allBands);
        }
        [HttpGet("/band/{id}")]
        public ActionResult find(int id)
        {
            Band.Find(id);
            List<Band> allBands = Band.GetAll();
            return View("bands", allBands);
        }

    }
}
