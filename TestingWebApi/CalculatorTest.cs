/*
 using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiTesting.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;


public class TestHttpRequest
{
    
    [Fact]
    public async Task GetAllFan_WhenSuccces()
    {
        var FanList = new List<string>
        {
            "Alialhadi DOTNET",
            "Sara alma2boora",
            "Emaaaaaaaaaaaaaaaaaan"
        };
        var mockhandler = MockHttpHandler<List<string>>.SetupGetRequest(FanList);
        var httpClient = new HttpClient(mockhandler.Object);
        
    }
}

*/