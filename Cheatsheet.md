# Dot Net #

## Create New .NET Empty Solution ##

`dotnet new sln -n <SOLUTION_PATH>`

## Create New .NET Projects ##

`dotnet new classlib -n <PROJECT_PATH>`

`dotnet new console -n <PROJECT_PATH>`

`dotnet new web -n <PROJECT_PATH>`

`dotnet new xunit -n <PROJECT_PATH>`

## Clean Solutions/Projects ##

`dotnet clean`

`dotnet clean <SOLUTION_PATH>`

`dotnet clean <PROJECT_PATH>`

## Add Project Reference(s) to Solution ##

`dotnet sln add <SOURCE_PROJECT_PATH>`

`dotnet sln <TARGET_SOLUTION_PATH> add <SOURCE_PROJECT_PATH>`

## Add Project Reference(s) to Project ##

`dotnet add reference <SOURCE_PROJECT_PATH>`

`dotnet add reference <SOURCE_PROJECT_PATH_1> <SOURCE_PROJECT_PATH_2>`

`dotnet add <TARGET_PROJECT_PATH> reference <SOURCE_PROJECT_PATH>`

`dotnet add <TARGET_PROJECT_PATH> reference <SOURCE_PROJECT_PATH_1> <SOURCE_PROJECT_PATH_2>`

## Remove Project Reference from Solution ##

`dotnet sln remove <SOURCE_PROJECT_PATH>`

`dotnet sln <TARGET_SOLUTION_PATH> remove <SOURCE_PROJECT_PATH>`

## Remove Project Reference from Project ##

`dotnet remove reference <SOURCE_PROJECT_PATH>`

`dotnet remove reference <SOURCE_PROJECT_PATH_1> <SOURCE_PROJECT_PATH_2>`

`dotnet remove <TARGET_PROJECT_PATH> reference <SOURCE_PROJECT_PATH>`

`dotnet remove <TARGET_PROJECT_PATH> reference <SOURCE_PROJECT_PATH_1> <SOURCE_PROJECT_PATH_2>`

# Create Nuget Package #

`dotnet pack -c Release --no-build --no-restore`

# GIT #

## Create a new GIT repository ##

`git init`

## Create a new GIT repository with a develop branch ##

`git init -b develop`

## Configure GIT repository ##

`git config user.email "john.doe@gmail.com"`

`git config user.name "first last"`

`git config --global user.email "john.doe@gmail.com"`

`git config --global user.name "first last"`

`git config --list --show-origin`
