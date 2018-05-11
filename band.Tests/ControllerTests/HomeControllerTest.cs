using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Controllers;
using ToDoList.Models;

namespace ToDoList.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
    [TestMethod]
    public void Index_ReturnsCorrectView_True()
    {
    ItemsController controller = new ItemsController();
    ActionResult indexView = controller.Index();
    Assert.IsInstanceOfType(indexView, typeof(ViewResult));
    }
}
}
