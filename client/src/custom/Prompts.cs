namespace client.custom;

public class Prompts
{
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
            StartPosition = FormStartPosition.CenterParent,
            Text = @"Details",
        };

        TextBox box1 = new TextBox()
        {
            PlaceholderText = placeholder1,
            Left = 30,
            Top = 10,
            Width = 340,
        };
        TextBox box2 = new TextBox()
        {
            PlaceholderText = placeholder2,
            Left = 30,
            Top = 40,
            Width = 340,
        };

        Button submitButton = new Button()
        {
            Text = @"submit",
            Left = 150,
            Width = 50,
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
