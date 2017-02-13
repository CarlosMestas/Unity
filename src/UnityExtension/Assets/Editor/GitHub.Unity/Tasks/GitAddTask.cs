using GitHub.Api;
using System;
using System.Collections.Generic;

namespace GitHub.Unity
{
    class GitAddTask : GitTask
    {
        private readonly string arguments;

        private GitAddTask(IEnumerable<string> files, Action onSuccess = null, Action onFailure = null)
            : base(str => onSuccess.SafeInvoke(), onFailure)
        {
            Guard.ArgumentNotNull(files, "files");

            arguments = "add ";
            arguments += " -- ";

            foreach (var file in files)
            {
                arguments += " " + file;
            }
        }

        public static void Schedule(IEnumerable<string> files, Action onSuccess = null, Action onFailure = null)
        {
            Tasks.Add(new GitAddTask(files, onSuccess, onFailure));
        }

        protected override void OnProcessOutputUpdate()
        {
            base.OnProcessOutputUpdate();

            // Always update
            StatusService.Instance.Run();
        }

        public override bool Blocking { get { return false; } }
        public override string Label { get { return "git add"; } }
        protected override string ProcessArguments { get { return arguments; } }
    }
}
