using Terminal.Gui;

namespace CmlLib_Minecraft_Launcher.UI;

internal sealed class LogWindow : Window
{
    private readonly TextView _textView;
    private readonly Button _exitButton;

    public LogWindow() : base("Game Log")
    {
        X = 0;
        Y = 0;
        Width = Dim.Fill();
        Height = Dim.Fill();

        var scheme = new ColorScheme
        {
            Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black),
            Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray)
        };
        ColorScheme = scheme;

        _textView = new TextView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 2,
            ReadOnly = true,
            WordWrap = false,
            ColorScheme = scheme
        };

        _exitButton = new Button("Exit")
        {
            X = Pos.AnchorEnd(10),
            Y = Pos.Bottom(_textView),
            Width = 8,
            ColorScheme = scheme
        };

        _exitButton.Clicked += () => InvokeOnUi(() => Application.RequestStop());

        Add(_textView, _exitButton);
    }

    public void AppendLine(string line)
    {
        InvokeOnUi(() =>
        {
            _textView.Text = _textView.Text + line + "\n";
            _textView.MoveEnd();
            Application.Refresh();
        });
    }

    public new void Clear()
    {
        InvokeOnUi(() =>
        {
            _textView.Text = string.Empty;
            Application.Refresh();
        });
    }

    private static void InvokeOnUi(Action action)
    {
        var loop = Application.MainLoop;
        if (loop == null)
        {
            action();
            return;
        }

        loop.Invoke(action);
    }
}

