using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Sikafon.Validation
{
    public interface IValidator
    {
        string Message { get; set; }
        bool Check(string value);
    }
    public interface IErrorStyle
    {
        void ShowError(View view, string message);
        void RemoveError(View view);
    }
    public class BasicErrorStyle : IErrorStyle
    {
        public void ShowError(View view, string message)
        {
            StackLayout layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);

            if (viewIndex + 1 < layout.Children.Count)
            {
                View sibling = layout.Children[viewIndex + 1];
                string siblingStyleId = view.Id.ToString();
                // Reuse the existing label
                if (sibling.StyleId == siblingStyleId)
                {
                    Label errorLabel = sibling as Label;
                    errorLabel.Text = message;
                    errorLabel.IsVisible = true;

                    return;
                }
            }
            // Add new label if none exists
            layout.Children.Insert(viewIndex + 1, new Label
            {
                Text = message,
                FontSize = 13,
                StyleId = view.Id.ToString(),
                TextColor = Color.Red
            });
        }

        public void RemoveError(View view)
        {
            StackLayout layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);

            if (viewIndex + 1 < layout.Children.Count)
            {
                View sibling = layout.Children[viewIndex + 1];
                string siblingStyleId = view.Id.ToString();

                if (sibling.StyleId == siblingStyleId)
                {
                    sibling.IsVisible = false;
                }
            }
        }
    }
    public class RequiredValidator : IValidator
    {
        public string Message { get; set; } = "This field is required";

        public bool Check(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
    public class LengthValidator : IValidator
    {
        public string Message { get; set; } = "Password should be 6 characters or more";

        public bool Check(string value)
        {
            return value.Length>5;
        }
    }
    public class EmailValidator : IValidator
    {
        public string Message { get; set; } = "Please enter a valid email";

        public bool Check(string value)
        {
            try
            {
                MailAddress m = new MailAddress(value);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
    public class FormatValidator : IValidator
    {
        public string Message { get; set; } = "Invalid format";
        public string Format { get; set; }

        public bool Check(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Regex format = new Regex(Format);

                return format.IsMatch(value);
            }
            else
            {
                return false;
            }
        }
    }
}
