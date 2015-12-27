﻿using System.IO;
using System.Text;
using System.Xml;
using static System.Console;

namespace XmlReaderAndWriterSample
{
    public class Program
    {
        private const string BooksFileName = "books.xml";
        private const string NewBooksFileName = "newbooks.xml";
        private const string ReadTextOption = "-r";
        private const string ReadElementContentOption = "-c";
        private const string ReadElementContentOption2 = "-c2";
        private const string ReadDecimalOption = "-d";
        private const string ReadAttributesOption = "-a";
        private const string WriteOption = "-w";

        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                ShowUsage();
                return;
            }

            switch (args[0])
            {
                case ReadTextOption:
                    ReadTextNodes();
                    break;
                case ReadElementContentOption:
                    ReadElementContent();
                    break;
                case ReadElementContentOption2:
                    ReadElementContent2();
                    break;
                case ReadDecimalOption:
                    ReadDecimal();
                    break;
                case ReadAttributesOption:
                    ReadAttributes();
                    break;
                case WriteOption:
                    WriterSample();
                    break;
                default:
                    ShowUsage();
                    break;
            }
        }

        private static void ShowUsage()
        {
            WriteLine("XmlReaderAndWriterSample options");
            WriteLine("\tOptions");
            WriteLine($"\t{ReadTextOption}\tRead Simple Text");
            WriteLine($"\t{ReadElementContentOption}\tRead Element Content");
            WriteLine($"\t{ReadDecimalOption}\tRead Decimal Content");
            WriteLine($"\t{ReadAttributesOption}\tRead Attributes");
            WriteLine($"\t{WriteOption}\tWrite");
        }

        public static void ReadTextNodes()
        {
            using (XmlReader reader = XmlReader.Create(BooksFileName))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        WriteLine(reader.Value);
                    }
                }
            }
        }

        public static void ReadElementContent()
        {
            using (XmlReader reader = XmlReader.Create(BooksFileName))
            {
                while (!reader.EOF)
                {
                    if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "title")
                    {
                        WriteLine(reader.ReadElementContentAsString());
                    }
                    else
                    {
                        // move on
                        reader.Read();
                    }
                }
            }
        }

        public static void ReadElementContent2()
        {
            using (XmlReader reader = XmlReader.Create(BooksFileName))
            {
                while (!reader.EOF)
                {
                    if (reader.MoveToContent() == XmlNodeType.Element)
                    {
                        try
                        {
                            WriteLine(reader.ReadElementContentAsString());
                        }
                        catch (XmlException)
                        {
                           reader.Read();
                        }
                    }
                    else
                    {
                        // move on
                        reader.Read();
                    }
                }
            }
        }

        public static void ReadDecimal()
        {
            using (XmlReader reader = XmlReader.Create(BooksFileName))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "price")
                        {
                            decimal price = reader.ReadElementContentAsDecimal();
                            WriteLine($"Current Price = {price}");
                            price += price * .25m;
                            WriteLine($"New price {price}");
                        }
                        else if (reader.Name == "title")
                        {
                            WriteLine(reader.ReadElementContentAsString());
                        }
                    }
                }
            }
        }

        public static void ReadAttributes()
        {
            using (XmlReader reader = XmlReader.Create(BooksFileName))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            WriteLine(reader.GetAttribute(i));
                        }
                    }
                }
            }
        }


        public static void WriterSample()
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true,
                Encoding = Encoding.UTF8,
                WriteEndDocumentOnClose = true
            };

            StreamWriter stream = File.CreateText(NewBooksFileName);
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument();
                //Start creating elements and attributes
                writer.WriteStartElement("book");
                writer.WriteAttributeString("genre", "Mystery");
                writer.WriteAttributeString("publicationdate", "2001");
                writer.WriteAttributeString("ISBN", "123456789");
                writer.WriteElementString("title", "Case of the Missing Cookie");
                writer.WriteStartElement("author");
                writer.WriteElementString("name", "Cookie Monster");
                writer.WriteEndElement();
                writer.WriteElementString("price", "9.99");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

        }
    }
}
