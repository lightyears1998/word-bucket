﻿using System.Diagnostics;
using Terminal.Gui;
using WordBucket.DataMaker.Parsers;

namespace WordBucket.DataMaker
{
    internal class MainWindow : Window
    {
        public MainWindow()
        {
            Title = "WordBucket DataMaker";

            var assemblyLocationLabel = new Label()
            {
                Text = $"Assembly Location: {AppConfig.AssemblyLocation}"
            };

            var workingDirectoryLabel = new Label()
            {
                Text = $"Working Directory: {AppConfig.WorkingDirectory}\n",
                Y = Pos.Bottom(assemblyLocationLabel)
            };

            var actionHeader = new Label()
            {
                Text = "Actions:",
                Y = Pos.Bottom(workingDirectoryLabel)
            };

            var openApplicationDataDirectoryButton = new Button()
            {
                Text = "Open Application Data Directory",
                Y = Pos.Bottom(actionHeader)
            };

            openApplicationDataDirectoryButton.Clicked += () =>
            {
                ProcessStartInfo info = new()
                {
                    FileName = "explorer.exe",
                    Arguments = AppConfig.ApplicationDataDirectory
                };
                Process.Start(info);
            };

            var createDatabaseButton = new Button()
            {
                Text = "Create Database",
                Y = Pos.Bottom(openApplicationDataDirectoryButton)
            };

            createDatabaseButton.Clicked += AppConfig.MigrateDatabases;

            var parseCollinsButton = new Button()
            {
                Text = "Parse Collins",
                Y = Pos.Bottom(createDatabaseButton)
            };

            parseCollinsButton.Clicked += () =>
            {
                var dialog = new OpenDialog("Select Collins Directory", "Select the directory contains Collins files.", null, OpenDialog.OpenMode.Directory);
                Application.Run(dialog);
                Application.RequestStop();
                CollinsParser.Parse(dialog.FilePath.ToString()!);
            };

            var exitButton = new Button()
            {
                Text = "Exit",
                Y = Pos.Bottom(parseCollinsButton)
            };

            exitButton.Clicked += () =>
            {
                Application.RequestStop();
            };

            Add(assemblyLocationLabel,
                workingDirectoryLabel,
                actionHeader,
                openApplicationDataDirectoryButton,
                createDatabaseButton,
                parseCollinsButton,
                exitButton);
        }
    }
}