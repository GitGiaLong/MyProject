namespace Core.Libraries.WPF.Interactivities
{
    internal sealed class NameResolvedEventArgs : EventArgs
    {
        private object oldObject;
        public object OldObject { get { return oldObject; } }

        private object newObject;
        public object NewObject { get { return newObject; } }

        public NameResolvedEventArgs(object oldObject, object newObject)
        {
            this.oldObject = oldObject;
            this.newObject = newObject;
        }
    }
}
