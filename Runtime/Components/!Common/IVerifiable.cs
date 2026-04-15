namespace KekwDetlef.LOST
{
    public interface IVerifiable<T>
    {
        public bool Verify(out T raw, out string errorMessage);
    }
}
