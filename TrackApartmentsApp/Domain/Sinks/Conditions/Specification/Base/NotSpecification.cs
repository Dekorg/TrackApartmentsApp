using TrackApartmentsApp.Domain.Sinks.Conditions.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Conditions.Specification.Base
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> item;

        public NotSpecification(ISpecification<T> item)
        {
            this.item = item;
        }

        public override bool IsSatisfiedBy(T another)
        {
            return !item.IsSatisfiedBy(another);
        }
    }
}
