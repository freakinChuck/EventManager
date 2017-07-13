using EventMaster.Storage.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EventMaster.Storage
{
    public class Workspace
    {
        public const string FILEENDING = ".evmn";
        public Workspace()
        {
            storageContainer = new StorageContainer();
        }
        private bool hasChanges = false;
        private string currentFilePath = string.Empty;
        private StorageContainer storageContainer;

        public static event EventHandler HasChangesChanged;
        public static event EventHandler<WorkspaceCancelEventArgs> AskSaveWorkspace;
        public static event EventHandler<WorkspacePathEventArgs> AskSavePathWorkspace;
        private static Workspace currentWorkspace;

        public static void LoadWorkspace()
        {
            WorkspacePathEventArgs args = new WorkspacePathEventArgs(null, false);
            AskSavePathWorkspace?.Invoke(null, args);
            if (!string.IsNullOrEmpty(args.Path))
            {
                LoadWorkspace(args.Path);
            }
        }
        public static void LoadWorkspace(string filePath)
        {
            Stream stream = new FileStream(filePath, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(StorageContainer));
            var container = (StorageContainer)serializer.Deserialize(stream);
            Workspace workspace = new Workspace();
            workspace.storageContainer = container;
            workspace.currentFilePath = filePath;
            currentWorkspace = workspace;
            SetHasChanges(false);
        }
        public static void CreateNewWorkspace()
        {
            if (currentWorkspace != null)
            {
                if (currentWorkspace.HasChanges)
                {
                    WorkspaceCancelEventArgs args = new WorkspaceCancelEventArgs(currentWorkspace);
                    AskSaveWorkspace?.Invoke(currentWorkspace, args);
                    if (args.Cancel)
                    {
                        return;
                    }
                    if (args.DoSave)
                    {
                        currentWorkspace.SaveWorkspace();
                    }
                }
                currentWorkspace = null;
            }

            var newWorkspace = new Workspace();
            currentWorkspace = newWorkspace;
            SetHasChanges(false);
        }

        public static void SaveCurrentWorkspace()
        {
            currentWorkspace.SaveWorkspace();
        }

        public void SaveWorkspace()
        {
            if (string.IsNullOrEmpty(this.currentFilePath))
            {
                WorkspacePathEventArgs args = new WorkspacePathEventArgs(this, true);
                AskSavePathWorkspace?.Invoke(this, args);
                if (string.IsNullOrEmpty(args.Path))
                {
                    throw new ArgumentException("Path");
                }
                this.currentFilePath = args.Path;
            }

            Stream stream = new FileStream(this.currentFilePath, FileMode.OpenOrCreate);
            XmlSerializer serializer = new XmlSerializer(typeof(StorageContainer));
            serializer.Serialize(stream, this.storageContainer);
            SetHasChanges(false);
        }

        private static void SetHasChanges(bool hasChanges)
        {
            hasChanges = false;
            HasChangesChanged?.Invoke(currentWorkspace, new EventArgs());
        }

        public static bool IsWorkspaceActive => currentWorkspace != null;
        public static StorageContainer CurrentData => currentWorkspace.storageContainer;

        public bool HasChanges
        {
            get { return hasChanges; }
        }
    }
}
