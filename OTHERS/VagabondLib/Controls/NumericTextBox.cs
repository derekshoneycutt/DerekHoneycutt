using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VagabondLib.Controls
{
    /// <summary>
    /// TextBox specially confined to only entering number values
    /// </summary>
    public class NumericTextBox : TextBox
    {
        /// <summary>
        /// The Value Property definition; a nullable decimal describing the value as seen by the user
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
          DependencyProperty.Register("Value",
                                        typeof(decimal?),
                                        typeof(NumericTextBox),
                                        new PropertyMetadata(null));

        /// <summary>
        /// Gets or Sets the Nullable Decimal Value to be displayed by the NumericTextBox
        /// </summary>
        public decimal? Value
        {
            get { return (decimal?)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                if (value != null)
                {
                    Text = value.ToString();
                }
                else
                {
                    Text = String.Empty;
                }
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            //Handle text input to limit to only numeric values; may include . and negative, but only where appropriate
            base.OnPreviewTextInput(e);
            if (String.IsNullOrWhiteSpace(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                var useText = Text + e.Text;
                if (String.Equals(useText.Substring(0, 1), "."))
                {
                    useText = "0" + useText;
                }
                if (!String.Equals(useText, "-"))
                {
                    decimal newValue = 0.0m;
                    e.Handled = !Decimal.TryParse(useText, out newValue);
                }
            }
        }

        /// <summary>
        /// Return a new string, with whitespace removed
        /// </summary>
        /// <param name="inStr">Original string to remove whitespace from</param>
        /// <returns>A newly built string, without the whitespace of the original</returns>
        private string RemoveWhitespace(string inStr)
        {
            return new string(inStr.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            //When the text in the TextBox is changed, ensure that it is valid

            //Null or Empty is always valid
            if (String.IsNullOrEmpty(Text))
            {
                Value = null;
                return;
            }

            //If there's any whitespace, remove it
            //This requires setting the cursor back to its expected position, because it is moved otherwise
            var useText = Text;
            if (useText.Any(c => Char.IsWhiteSpace(c)))
            {
                if (!String.IsNullOrWhiteSpace(useText))
                {
                    useText = RemoveWhitespace(useText);
                    var preCaretIndex = CaretIndex;
                    var startStrLen = RemoveWhitespace(Text.Substring(0, preCaretIndex));
                    Text = useText;
                    CaretIndex = startStrLen.Length;
                }
                else
                {
                    Text = Value.ToString();
                    useText = Text;
                    CaretIndex = useText.Length;
                }
            }

            //We can now try to parse the string int a decimal, and check if the changed text is valid
            if (!String.IsNullOrEmpty(useText))
            {
                if (String.Equals(useText.Substring(0, 1), "."))
                {
                    useText = "0" + useText;
                }
            }
            decimal trueValue = 0m;
            if (Decimal.TryParse(useText, out trueValue))
            {
                //If it's valid, set the Value property to the new value
                SetValue(ValueProperty, trueValue);
                base.OnTextChanged(e);
            }
            else
            {
                //If the string is not valid, not null or empty, and not a '.' or '-' (decimal point or negative) only, then set it back to its original value
                if (!String.IsNullOrEmpty(Text) &&
                    !String.Equals(Text, ".") &&
                    !String.Equals(Text, "-"))
                {
                    Text = Value.ToString();
                    CaretIndex = Text.Length;
                }
            }
        }
    }
}
