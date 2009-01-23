using System;
using System.Collections.Generic;
using System.Text;
using Merlin;
using MerlinStepLibrary;
using System.Windows.Forms;

namespace CCNetConfig.UI
{
    /// <summary>
    /// Displays a block of text.
    /// </summary>
    public class TextDisplayStep
        : TemplateStep
    {
        #region Constructors
        /// <summary>
        /// Initialise a new <see cref="TextDisplayStep"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public TextDisplayStep(string message)
            : base()
        {
            var textBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Text = message
            };
            textBox.Select(0, 0);
            var panel = new Panel
            {
                Padding = new Padding(10)
            };
            panel.Controls.Add(textBox);
            UI = panel;
        }

        /// <summary>
        /// Initialise a new <see cref="TextDisplayStep"/> with a title.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The title of the step.</param>
        public TextDisplayStep(string message, string title)
            : this(message)
        {
            Title = title;
        }
        #endregion
    }
}
