using System;

namespace EventMaster.Storage
{
    public class WorkspacePathEventArgs : WorkspaceEventArgs
    {
        public WorkspacePathEventArgs(Workspace workspace, bool isSave)
            :base(workspace)
        {
            this.IsSave = isSave;
        }

        public string Path { get; set; }
        public bool IsSave { get; private set; }
    }
}