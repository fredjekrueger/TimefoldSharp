using System.Collections.Concurrent;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Change;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class BasicPlumbingTermination : AbstractTermination
    {
        protected readonly bool daemon;
        protected bool terminatedEarly = false;
        protected bool problemFactChangesBeingProcessed = false;

        protected BlockingCollection<ProblemChangeAdapter> problemFactChangeQueue = new BlockingCollection<ProblemChangeAdapter>();

        public BasicPlumbingTermination(bool daemon)
        {
            this.daemon = daemon;
        }

        public override bool IsSolverTerminated(SolverScope solverScope)
        {
            //JDEFJDEF
            /*   var t = Thread.CurrentThread.ThreadState;
               if (t == ThreadState.Stopped ||t == ThreadState.StopRequested || t == ThreadState.AbortRequested || t == ThreadState.Stopped
                   || t == ThreadState.Aborted// Does not clear the interrupted flag
                                                          // Avoid duplicate log message because this method is called twice:
                                                          // - in the phase step loop (every phase termination bridges to the solver termination)
                                                          // - in the solver's phase loop
                  && !terminatedEarly)
               {
                   terminatedEarly = true;
               }
               return terminatedEarly || problemFactChangeQueue.Any();*/

            return problemFactChangeQueue.Count > 0;
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {

        }

        public void ResetTerminateEarly()
        {
            terminatedEarly = false;
        }

        public void EndProblemFactChangesProcessing()
        {
            problemFactChangesBeingProcessed = false;
        }

        public bool WaitForRestartSolverDecision()
        {
            if (!daemon)
            {
                return problemFactChangeQueue.Count > 0 && !terminatedEarly;
            }
            else
            {
                while (problemFactChangeQueue.Count == 0 && !terminatedEarly)
                {
                    try
                    {
                        throw new NotImplementedException();
                        //wait();
                    }
                    catch (Exception e)
                    {
                        throw new NotImplementedException();
                        //Thread.currentThread().interrupt();
                        throw new Exception("Solver thread was interrupted during Object.wait().", e);
                    }
                }
                return !terminatedEarly;
            }
        }

        public bool TerminateEarly()
        {
            bool terminationEarlySuccessful;
            terminationEarlySuccessful = !terminatedEarly;
            terminatedEarly = true;
            return terminationEarlySuccessful;
        }

        public bool IsEveryProblemFactChangeProcessed()
        {
            return problemFactChangeQueue.Count == 0 && !problemFactChangesBeingProcessed;
        }

        internal bool IsTerminateEarly()
        {
            return terminatedEarly;
        }

        public BlockingCollection<ProblemChangeAdapter> StartProblemFactChangesProcessing()
        {
            problemFactChangesBeingProcessed = true;
            return problemFactChangeQueue;
        }

        public override bool IsPhaseTerminated(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope)
        {
            throw new Exception(nameof(BasicPlumbingTermination)
                + " configured only as solver termination."
                + " It is always bridged to phase termination.");
        }

        public override double CalculateSolverTimeGradient(SolverScope solverScope)
        {
            return -1.0;
        }
    }
}
