// Enables callbacks to be dispatched from any thread and be handled on
// the thread that owns the instance to this class (eg. the UIThread).
internal class ThreadDispatcher
{
    private int ownerThreadId;
    private System.Collections.Generic.Queue<System.Action> queue =
        new System.Collections.Generic.Queue<System.Action>();

    public ThreadDispatcher()
    {
        ownerThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
    }

    private class CallbackStorage<TResult>
    {
        public TResult Result { get; set; }
        public System.Exception Exception { get; set; }
    }

    // Triggers the job to run on the main thread, and waits for it to finish.
    public TResult Run<TResult>(System.Func<TResult> callback)
    {
        if (ManagesThisThread())
        {
            return callback();
        }

        var waitHandle = new System.Threading.EventWaitHandle(
            false, System.Threading.EventResetMode.ManualReset);

        var result = new CallbackStorage<TResult>();
        lock (queue)
        {
            queue.Enqueue(() => {
                try
                {
                    result.Result = callback();
                }
                catch (System.Exception e)
                {
                    result.Exception = e;
                }
                finally
                {
                    waitHandle.Set();
                }
            });
        }
        waitHandle.WaitOne();
        if (result.Exception != null) throw result.Exception;
        return result.Result;
    }

    // Determines whether this thread is managed by this instance.
    internal bool ManagesThisThread()
    {
        return System.Threading.Thread.CurrentThread.ManagedThreadId == ownerThreadId;
    }

    // This dispatches jobs queued up for the owning thread.
    // It must be called regularly or the threads waiting for job will be
    // blocked.
    public void PollJobs()
    {
        System.Diagnostics.Debug.Assert(ManagesThisThread());

        System.Action job;
        while (true)
        {
            lock (queue)
            {
                if (queue.Count > 0)
                {
                    job = queue.Dequeue();
                }
                else
                {
                    break;
                }
            }
            job();
        }
    }
}