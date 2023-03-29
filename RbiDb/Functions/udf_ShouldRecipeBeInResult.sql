CREATE FUNCTION [dbo].udf_ShouldRecipeBeInResult
(
	@isPublic BIT,
	@recipeOwnerId INT,
	@userId INT,
	@includePublicRecipes BIT,
	@includePrivateRecipesOfUser BIT
)
RETURNS BIT
BEGIN
	IF (@includePublicRecipes = 1 AND @isPublic = 1) OR
		(@userId = @recipeOwnerId AND (@isPublic = 1 OR @includePrivateRecipesOfUser = 1))
		RETURN 1;

	RETURN 0;
END
