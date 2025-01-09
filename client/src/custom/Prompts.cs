namespace client.custom;

public class Prompts
{
    public static Form SingleInputForm(string placeholder1, Action<string> submitCallback)
    {
        Form prompt = new Form()
        {
            Width = 400,
            Height = 120,
            Padding = Padding.Empty,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MinimizeBox = false,
            MaximizeBox = false,
            Text = @"Details",
        };

        TextBox box1 = new TextBox()
        {
            PlaceholderText = placeholder1,
            Left = 30,
            Top = 10,
            Width = 325,
        };
        Button submitButton = new Button()
        {
            Text = @"submit",
            Width = 50,
            Left = 150,
            Top = 40,
        };
        prompt.AcceptButton = submitButton;

        submitButton.Click += (s, e) =>
        {
            prompt.Close();
            submitCallback.Invoke(box1.Text);
        };

        prompt.Controls.Add(box1);
        prompt.Controls.Add(submitButton);

        return prompt;
    }

    public static Form DoubleInputPrompt(
        string placeholder1,
        string placeholder2,
        Action<string, string> submitCallback
    )
    {
        Form prompt = new Form()
        {
            Width = 400,
            Height = 160,
            Padding = Padding.Empty,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MinimizeBox = false,
            MaximizeBox = false,
            Text = @"Details",
        };

        TextBox box1 = new TextBox()
        {
            PlaceholderText = placeholder1,
            Left = 30,
            Top = 10,
            Width = 325,
        };
        TextBox box2 = new TextBox()
        {
            PlaceholderText = placeholder2,
            Left = 30,
            Top = 40,
            Width = 325,
        };

        Button submitButton = new Button()
        {
            Text = @"submit",
            Width = 50,
            Left = 150,
            Top = 80,
        };
        prompt.AcceptButton = submitButton;

        submitButton.Click += async (s, e) =>
        {
            prompt.Close();
            await Task.Factory.StartNew(() => submitCallback.Invoke(box1.Text, box2.Text));
        };

        prompt.Controls.Add(box1);
        prompt.Controls.Add(box2);
        prompt.Controls.Add(submitButton);

        return prompt;
    }
}
