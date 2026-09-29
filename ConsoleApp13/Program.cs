using System;
using System.Globalization;

namespace DeliveryService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Добро пожаловать в симулятор службы доставки ===");

           
            decimal basePrice = 0;
            while (true)
            {
                Console.Write("\nВведите базовую стоимость доставки (например, 20): ");
                string priceInput = Console.ReadLine();

                if (decimal.TryParse(priceInput, NumberStyles.Any, CultureInfo.CurrentCulture, out basePrice) && basePrice >= 0)
                {
                    break;
                }
                if (decimal.TryParse(priceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out basePrice) && basePrice >= 0)
                {
                    break;
                }

                Console.WriteLine("Ошибка! Введите корректное положительное число.");
            }

           
            Delivery standard = CreateDeliveryWithInput("стандартной");
            Delivery express = CreateDeliveryWithInput("экспресс");
            Delivery international = CreateDeliveryWithInput("международной");

            
            standard.CalculatePrice(basePrice);
            express.CalculatePrice(basePrice);
            international.CalculatePrice(basePrice);

            
            Console.WriteLine("\n=== Результаты расчёта ===");
            Console.WriteLine(standard.ToString());
            Console.WriteLine(express.ToString());
            Console.WriteLine(international.ToString());

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static Delivery CreateDeliveryWithInput(string deliveryType)
        {
            while (true)
            {
                Console.Write($"Введите название для {deliveryType} доставки: ");
                string name = Console.ReadLine();

                try
                {
                    switch (deliveryType)
                    {
                        case "стандартной":
                            return new StandardDelivery(name);
                        case "экспресс":
                            return new ExpressDelivery(name);
                        case "международной":
                            return new InternationalDelivery(name);
                        default:
                            throw new ArgumentException("Неизвестный тип доставки");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message} Попробуйте ещё раз.");
                }
            }
        }
    }

   
    public abstract class Delivery
    {
        protected string name;
        protected decimal price;

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название доставки не может быть пустым.");
                }
                if (value.Length > 20)
                {
                    throw new ArgumentException("Название доставки не должно превышать 20 символов.");
                }
                name = value;
            }
        }

        public decimal Price
        {
            get => price;
        }

        protected Delivery(string name)
        {
            Name = name;
        }

        public abstract void CalculatePrice(decimal basePrice);

        public override string ToString()
        {
            return $"Тип доставки: {Name,-20} | Итоговая стоимость: {Price:C}";
        }
    }

   
    public class StandardDelivery : Delivery
    {
        public StandardDelivery(string name) : base(name) { }

        public override void CalculatePrice(decimal basePrice)
        {
            price = basePrice * 1.0m;
        }
    }

    
    public class ExpressDelivery : Delivery
    {
        public ExpressDelivery(string name) : base(name) { }

        public override void CalculatePrice(decimal basePrice)
        {
            price = basePrice * 2.5m;
        }
    }

  
    public class InternationalDelivery : Delivery
    {
        public InternationalDelivery(string name) : base(name) { }

        public override void CalculatePrice(decimal basePrice)
        {
            price = basePrice * 4.0m;
        }
    }
}
