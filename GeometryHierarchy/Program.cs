using GeometryHierarchy.Entities;

Console.WriteLine("Enter the radius of the circle:");
double circleRadius = Convert.ToDouble(Console.ReadLine());

Geometry circle = new Circle(circleRadius);
Console.WriteLine($"Circle Area: {circle.CalculateArea()}");

Console.WriteLine("Enter the length of the rectangle:");
double rectangleLength = Convert.ToDouble(Console.ReadLine());
Console.WriteLine("Enter the width of the rectangle:");
double rectangleWidth = Convert.ToDouble(Console.ReadLine());

Geometry rectangle = new Rectangle(rectangleLength, rectangleWidth);
Console.WriteLine($"Rectangle Area: {rectangle.CalculateArea()}");