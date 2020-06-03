using CommandLine;
using Dapper;
using MySql.Data.MySqlClient;
using System;

namespace MySQL_dockerdb
{
    /// <summary>
    /// Simple program to push data from TeamCity buildstep to database
    /// database connection is setup from environment variable
    /// configurable in TeamCity.
    /// </summary>
    /// <author>Daan Eekhof</author>
    internal class Program
    {
        /// <summary>
        /// Main method to start the console app
        /// parses and arguments and checks if there are arguments.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // Check if there are arguments passed.
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments passed");
                throw new NullReferenceException();
            }
            else
            {
                try
                {
                    // Parse the command line variables.
                    Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
                    {
                        SetData(o);
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
            }
        }

        /// <summary>
        /// Upload data to the database.
        /// </summary>
        /// <param name="args"></param>
        public static void SetData(Options args)
        {
            using (MySqlConnection _connection = new MySqlConnection(Environment.GetEnvironmentVariable("DatabaseConnection")))
            {
                var dockerDate = DateTime.Now.ToString("yyyy-MM-dd H:m:s");

                /// Change username to n/a if the build is triggered by Git.
                if (args.setTriggerUsername == "" || args.setTriggerUsername == null)
                    args.setTriggerUsername = "n/a";

                // Call method when the liveStatus is set to 'live'.
                if (args.setLiveStatus == "live")
                    ChangeLiveStatus(args);

                _connection.Query(
                    @"INSERT INTO docker
                    (
                        buildNr,
                        imageName,
                        imageTag,
                        projectName,
                        projectID,
                        agentName,
                        triggerReason,
                        triggerUsername,
                        commitHash,
                        vcsRoot,
                        vcsBranch,
                        dockerVersion,
                        liveStatus,
                        dockerDate)
                    VALUES
                    (
                        @setBuildNr,
                        @setImageName,
                        @setImageTag,
                        @setProjectName,
                        @setProjectID,
                        @setAgentName,
                        @setTriggerReason,
                        @setTriggerUsername,
                        @setCommitHash,
                        @setVcsRoot,
                        @setVcsBranch,
                        @setDockerVersion,
                        @setLiveStatus,
                        @dockerDate
                    );",
                    new
                    {
                        args.setBuildNr,
                        args.setImageName,
                        args.setImageTag,
                        args.setProjectName,
                        args.setProjectID,
                        args.setAgentName,
                        args.setTriggerReason,
                        args.setTriggerUsername,
                        args.setCommitHash,
                        args.setVcsRoot,
                        args.setVcsBranch,
                        args.setDockerVersion,
                        args.setLiveStatus,
                        dockerDate
                    });
            };
        }

        /// <summary>
        /// As we can't get the current live status from AWS we choose to set the new image as
        /// live from within this program.
        /// </summary>
        /// <param name="args"></param>
        public static void ChangeLiveStatus(Options args)
        {
            using (MySqlConnection _connection = new MySqlConnection(Environment.GetEnvironmentVariable("DatabaseConnection")))
            {
                var imageTag = "version-%";

                if (!args.setImageTag.Contains("version-"))
                    imageTag = args.setImageTag;

                _connection.Query(
                    @"UPDATE docker
                    SET liveStatus = 'deprecated'
                    WHERE
                        liveStatus = 'live' AND
                        projectName = @setProjectName AND
                        projectID = @setProjectID AND
                        imageName = @setImageName AND
                        imageTag LIKE @imageTag",
                    new { args.setProjectName, args.setProjectID, args.setImageName, imageTag });
            };
        }
    }

    /// <summary>
    /// Class to initialize Command Line options
    /// used for easy casting of variables.
    /// </summary>
    internal class Options
    {
        [Option('b', "buildnr", Required = true, HelpText = "Set build number.")]
        public int setBuildNr { get; set; }

        [Option('i', "imagename", Required = true, HelpText = "Set image name.")]
        public string setImageName { get; set; }

        [Option('t', "imagetag", Required = true, HelpText = "Set image tag.")]
        public string setImageTag { get; set; }

        [Option('p', "projectname", Required = true, HelpText = "Set project name.")]
        public string setProjectName { get; set; }

        [Option('d', "projectid", Required = true, HelpText = "Set project id.")]
        public string setProjectID { get; set; }

        [Option('a', "agentname", Required = true, HelpText = "Set agent name.")]
        public string setAgentName { get; set; }

        [Option('t', "triggerreason", Required = true, HelpText = "Set trigger reason.")]
        public string setTriggerReason { get; set; }

        [Option('u', "triggerusername", Required = true, HelpText = "Set trigger username.")]
        public string setTriggerUsername { get; set; }

        [Option('c', "commithash", Required = true, HelpText = "Set commit hash.")]
        public string setCommitHash { get; set; }

        [Option('v', "vcsroot", Required = true, HelpText = "Set vcs root.")]
        public string setVcsRoot { get; set; }

        [Option('q', "vcsbranch", Required = true, HelpText = "Set vcs branch.")]
        public string setVcsBranch { get; set; }

        [Option('v', "dockerversion", Required = true, HelpText = "Set docker version.")]
        public string setDockerVersion { get; set; }

        [Option('l', "livestatus", Required = true, HelpText = "Set live status.")]
        public string setLiveStatus { get; set; }
    }
}