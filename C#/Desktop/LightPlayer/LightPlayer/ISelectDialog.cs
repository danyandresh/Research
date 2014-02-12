using System;

namespace LightPlayer
{
    public interface ISelectDialog
    {
        Tuple<DialogResult, string> Show();
    }
}
