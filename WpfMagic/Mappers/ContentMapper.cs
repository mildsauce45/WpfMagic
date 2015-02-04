using System;

namespace WpfMagic.Mappers
{
    public abstract class ContentMapper
    {
        public Type ControlType { get; private set; }
        public string ContentProperty { get; private set; }

        public ContentMapper(Type controlType, string contentProperty)
        {
            this.ControlType = controlType;
            this.ContentProperty = contentProperty;
        }        
    }
}
