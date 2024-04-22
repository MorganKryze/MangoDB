﻿using ConsoleAppVisuals;
using ConsoleAppVisuals.AnimatedElements;
using ConsoleAppVisuals.Enums;
using ConsoleAppVisuals.InteractiveElements;
using ConsoleAppVisuals.PassiveElements;

namespace MangoDB;

class Program
{
    public static Profile user = Profile.None;

    static void Main()
    {
        Window.Open();

        Setup();

        Authentication();

        Window.Close();
    }

    static void Setup()
    {
        Title title = new("Mango DB", font: Font.Ghost);
        Header header = new(centerText: "Welcome to MangoDB");
        Footer footer = new("[ESC] Back", "[Z|↑] Up   [S|↓] Down", "[ENTER] Select");
        FakeLoadingBar startingLoadingBar = new("[ Starting the app ... ]");
        Window.AddElement(title, header, footer, startingLoadingBar);
        Window.Render();
        Window.ActivateElement(startingLoadingBar);
    }

    static void Authentication()
    {
        Prompt id = new("Please enter your ID (email or username):");
        Dialog wrongId =
            new(
                ["Wrong ID", string.Empty, "The ID you entered is not in the database."],
                "Quit",
                "Try again"
            );
        Window.AddElement(id, wrongId);

        while (true)
        {
            Window.ActivateElement(id);
            var idResponse = id.GetResponse();
            // ! DEBUG: Validation to be replaced with db query
            // TODO: Replace with db query
            string idExpected = "admin";
            if (idResponse!.Value != idExpected)
            {
                Window.ActivateElement(wrongId);
                var wrongIdResponse = wrongId.GetResponse();
                if (wrongIdResponse!.Value == DialogOption.Left)
                {
                    return;
                }
                continue;
            }
            // ! DEBUG: Validation to be replaced with db query
            break;
        }

        Prompt password = new("Please enter your password:", style: PromptInputStyle.Secret);
        Dialog wrongPassword =
            new(
                ["Wrong Password", string.Empty, "The password you entered is incorrect."],
                "Quit",
                "Try again"
            );
        Window.AddElement(password, wrongPassword);

        while (true)
        {
            Window.ActivateElement(password);
            var passwordResponse = password.GetResponse();
            // ! DEBUG: Validation to be replaced with db query
            // TODO: Replace with db query
            string passwordExpected = "secret";
            if (passwordResponse!.Value != passwordExpected)
            {
                Window.ActivateElement(wrongPassword);
                var wrongPasswordResponse = wrongPassword.GetResponse();
                if (wrongPasswordResponse!.Value == DialogOption.Left)
                {
                    return;
                }
                continue;
            }
            // ! DEBUG: Validation to be replaced with db query
            break;
        }

        FakeLoadingBar profileLoadingBar = new("[ Loading profile... ]");
        Window.AddElement(profileLoadingBar);
        Window.ActivateElement(profileLoadingBar);
    }
}