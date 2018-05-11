using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandList.Models;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace BandList.Tests
{

[TestClass]
public class BandTest : IDisposable
 {
    public BandTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
    }
    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }

    [TestMethod]
    public void Getdatasbase()
    {
      //Arrange, Act
      int result = Band.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void testcreate()
    {
      //Arrange, Act
      Band firstItem = new Band("era");
      Band secondItem = new Band("era");

      //Assert
      Assert.AreEqual(firstItem, secondItem);
    }

    [TestMethod]
    public void todatabase()
    {
      //Arrange
      Band testItem = new Band("era");
      testItem.Save();

      //Act
      List<Band> result = Band.GetAll();
      List<Band> testList = new List<Band>{testItem};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void correctid()
    {
      //Arrange
      Band testItem = new Band("era");
      testItem.Save();

      //Act
      Band savedItem = Band.GetAll()[0];

      int result = savedItem.GetId();
      int testId = testItem.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void findindatabase()
    {
      //Arrange
      Band testItem = new Band("era");
      testItem.Save();

      //Act
      Band result = Band.Find(testItem.GetId());

      //Assert
      Assert.AreEqual(testItem, result);
    }
    [TestMethod]
    public void testjoinandget()
    {
      //Arrange
      Band testItem = new Band("era");
      testItem.Save();

      Venue testVenue = new Venue("Home stuff");
      testVenue.Save();

      //Act
      testItem.AddVenue(testVenue);

      List<Venue> result = testItem.GetVenues();
      List<Venue> testList = new List<Venue>{testVenue};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void GetVenues()
    {
      //Arrange
      Band testItem = new Band("era");
      testItem.Save();

      Venue testVenue1 = new Venue("seattle");
      testVenue1.Save();

      Venue testVenue2 = new Venue("vancouver");
      testVenue2.Save();

      //Act
      testItem.AddVenue(testVenue1);
      List<Venue> result = testItem.GetVenues();
      List<Venue> testList = new List<Venue> {testVenue1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void testdelete()
    {
      //Arrange
      Band testItem = new Band("era");
      testItem.Save();
      List<Band> result = Band.GetAll();
      foreach (var band in result)
      {
       Console.WriteLine("the id number " + band.GetId());
      }
      //act
      Band.Delete(1);
      

      Assert.AreEqual(0, result.Count);
    }

  }
}
