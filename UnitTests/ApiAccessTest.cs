using Moq;
using RbiFrontend.ApiAccess;
using RbiShared.DTOs;
using RbiShared.SearchObjects;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace UnitTests;

public class ApiAccessTest
{
    [Fact]
    public void RecipeSearchToQueryStringTest()
    {
        var search = new RecipeSearch
        {
            TagNames = new[] { "T1", "T2" },
            UserId = 1,
            IncludePrivateRecipesOfUser = true,
            IncludePublicRecipes = false,
            ExactMatch = false,
            Offset = 0,
            Limit = 100
        };
        var queryString = @"TagNames=T1&TagNames=T2&UserId=1&IncludePrivateRecipesOfUser=True&IncludePublicRecipes=False&ExactMatch=False&Offset=0&Limit=100";

        Assert.Equal(queryString, UrlUtils.ObjectToQueryString(search));
    }
}