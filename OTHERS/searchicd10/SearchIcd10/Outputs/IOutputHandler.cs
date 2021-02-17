
namespace SearchIcd10.Outputs
{
    /// <summary>
    /// Interface defining methods for placing the final output of the product
    /// </summary>
    public interface IOutputHandler
    {
        /// <summary>
        /// Puts the output where it is anticipated
        /// </summary>
        /// <param name="outputText">The text to output</param>
        /// <returns>True if operation is successful at placing output</returns>
        bool PutOutputText(string outputText);

        /// <summary>
        /// Switch to the existing window, if applicable to the output
        /// </summary>
        void SwitchToExisting();
    }
}
