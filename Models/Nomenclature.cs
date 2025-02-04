using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Nomenclature : IModel, IEquatable<Nomenclature>
    {
        [DisplayName("КодНоменклатуры")]
        public int ID { get; }
        [DisplayName("Наименование")]
        public string Name { get; }
        [DisplayName("Тип")]
        public NomenclatureTypeValues Type { get; }
        [DisplayName("Категория")]
        public NomenclatureCategoriesValues CategoryCargo { get; }
        [DisplayName("Длина")]
        public float? Length { get; }
        [DisplayName("Ширина")]
        public float? Width { get; }
        [DisplayName("Высота")]
        public float? Height { get; }
        [DisplayName("Вес")]
        public float? Weight { get; }
        [DisplayName("ЕдиницаИзмерения")]
        public NomenclatureMeasureValues Unit { get; }
        [DisplayName("Упаковка")]
        public NomenclaturePackingValues Pack { get; }
        [DisplayName("ТребованияКТемпературе")]
        public string? NeedTemperature { get; }
        [DisplayName("Опасность")]
        public NomenclatureDangerousValues DangerousClass { get; }
        [DisplayName("Производитель")]
        public string? Manufacturer { get; }
        [DisplayName("СрокГодности")]
        public int? ExpiryDate { get; }

        public Nomenclature() 
        {
            ID = 0;
            Name = string.Empty;
            Type = NomenclatureTypeValues.Other;
            CategoryCargo = NomenclatureCategoriesValues.Standard;
            Length = null;
            Width = null;
            Height = null;
            Weight = null;
            Unit = NomenclatureMeasureValues.sht;
            Pack = NomenclaturePackingValues.None;
            NeedTemperature = null;
            DangerousClass = NomenclatureDangerousValues.None;
            Manufacturer = null;
            ExpiryDate = null;
        }

        public Nomenclature(
            int iD,
            string name,
            string type,
            string categoryCargo,
            float? length,
            float? width,
            float? height,
            float? weight,
            string unit,
            string pack,
            string? needTemperature,
            string dangerousClass,
            string? manufacturer,
            int? expiryDate)
        {
            ID = iD;
            Name = name;
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            NeedTemperature = needTemperature;
            Manufacturer = manufacturer;
            ExpiryDate = expiryDate;

            Type = type switch
            {
                "Продукты" => NomenclatureTypeValues.Products,
                "Стройматериалы" => NomenclatureTypeValues.Buildings,
                "Электроника" => NomenclatureTypeValues.Electronics,
                "Химия" => NomenclatureTypeValues.Chemistry,
                "Мебель" => NomenclatureTypeValues.Furniture,
                "Другое" => NomenclatureTypeValues.Other,
                _ => throw new NotImplementedException()
            };
            CategoryCargo = categoryCargo switch
            {
                "Опасные" => NomenclatureCategoriesValues.Dangerous,
                "Скоропортящиеся" => NomenclatureCategoriesValues.Perishable,
                "Стандартные" => NomenclatureCategoriesValues.Standard,
                "Тяжелые" => NomenclatureCategoriesValues.Heavy,
                "Габаритные" => NomenclatureCategoriesValues.Oversize,
                _ => throw new NotImplementedException()
            };
            Unit = unit switch
            {
                "шт" => NomenclatureMeasureValues.sht,
                "кг" => NomenclatureMeasureValues.kg,
                "м" => NomenclatureMeasureValues.m,
                _ => throw new NotImplementedException()
            };
            DangerousClass = dangerousClass switch
            {
                "Класс 1" => NomenclatureDangerousValues.One,
                "Класс 2" => NomenclatureDangerousValues.Two,
                "Класс 3" => NomenclatureDangerousValues.Three,
                "Не опасный" => NomenclatureDangerousValues.None,
                _ => NomenclatureDangerousValues.Null
            };
            Pack = pack switch
            {
                "Паллеты" => NomenclaturePackingValues.Pallet,
                "Коробки" => NomenclaturePackingValues.Box,
                "Контейнеры" => NomenclaturePackingValues.Container,
                "Бочки" => NomenclaturePackingValues.Barrel,
                "Мешки" => NomenclaturePackingValues.Bag,
                "Без упаковки" => NomenclaturePackingValues.None,
                _ => NomenclaturePackingValues.Null
            };
        }

        public Nomenclature(
            int iD,
            string name,
            NomenclatureTypeValues type,
            NomenclatureCategoriesValues categoryCargo,
            float? length,
            float? width,
            float? height,
            float? weight,
            NomenclatureMeasureValues unit,
            NomenclaturePackingValues pack,
            string? needTemperature,
            NomenclatureDangerousValues dangerousClass,
            string? manufacturer,
            int? expiryDate)
        {
            ID = iD;
            Name = name;
            Type = type;
            CategoryCargo = categoryCargo;
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            Unit = unit;
            Pack = pack;
            NeedTemperature = needTemperature;
            DangerousClass = dangerousClass;
            Manufacturer = manufacturer;
            ExpiryDate = expiryDate;
        }

        public static string GetTable() => "Номенклатура";
        public static string GetSelectorID() => "КодНоменклатуры";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодНоменклатуры",
                "Наименование",
                "Тип",
                "Категория",
                "Длина",
                "Ширина",
                "Высота",
                "Вес",
                "ЕдиницаИзмерения",
                "Упаковка",
                "ТребованияКТемпературе",
                "Опасность",
                "Производитель",
                "СрокГодности"
            };
        }

        public bool Equals(Nomenclature? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Nomenclature);

        public override int GetHashCode() => HashCode.Combine(ID, Name);
    }
}