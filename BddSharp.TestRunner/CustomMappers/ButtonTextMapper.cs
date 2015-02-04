using BddSharp.TestRunner.Controls;
using WpfMagic.Mappers;

namespace BddSharp.TestRunner.CustomMappers
{
    public class ButtonTextMapper : ContentMapper
    {
        public ButtonTextMapper()
            : base(typeof(Button), "Text")
        {
        }
    }
}
