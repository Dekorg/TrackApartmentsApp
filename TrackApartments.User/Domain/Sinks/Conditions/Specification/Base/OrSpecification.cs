using TrackApartments.User.Domain.Sinks.Conditions.Specification.Base.Abstract;

namespace TrackApartments.User.Domain.Sinks.Conditions.Specification.Base
{
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> leftSpecification;
        private readonly ISpecification<T> rightSpecification;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            leftSpecification = left;
            rightSpecification = right;
        }

        public override bool IsSatisfiedBy(T item)
        {
            return leftSpecification.IsSatisfiedBy(item)
                   || rightSpecification.IsSatisfiedBy(item);
        }
    }
}
