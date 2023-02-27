using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TASK.Controllers;
using TASK.Models;

namespace TASK;

[TestFixture]
public class FileControllerTests
{
    [Test]
    public void Post_Returns_Correct_Type()
    {
        var content = "random info";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile fileTest = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        var appContext = A.Fake<ApplicationContext>();
        var newFileController = new FileController(appContext);
        var result = newFileController.PostFile(fileTest);

        Assert.IsInstanceOf(typeof(ActionResult), result);
    }

    
    [Test]
    public void Wont_Parse_Wrong_Data()
    {
        var content = "2022-03;1744;1632,472";
        var fileName = "test1.csv";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile fileTest1 = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        var appContext = A.Fake<ApplicationContext>();
        var newFileController = new FileController(appContext);
        var result = newFileController.PostFile(fileTest1);
        
        Assert.IsNotInstanceOf(typeof(OkResult), result);
    }
    
    [Test]
    public void Wont_Parse_Negative_Value()
    {
        var content = "2022-03-18_09-18-17;1744;-124,6";
        var fileName = "test2.csv";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile fileTest2 = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        var appContext = A.Fake<ApplicationContext>();
        var newFileController = new FileController(appContext);
        var result = newFileController.PostFile(fileTest2);
        
        Assert.IsNotInstanceOf(typeof(OkResult), result);
    }
}