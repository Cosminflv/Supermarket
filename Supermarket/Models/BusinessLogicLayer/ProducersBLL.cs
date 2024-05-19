using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models.BusinessLogicLayer
{
    internal class ProducersBLL
    {
        public ProducersBLL()
        {
            Producers = GetAllProducers();
        }

        private SupermarketEntities context = new SupermarketEntities();

        public ObservableCollection<Producatori> Producers { get; set; }

        public ObservableCollection<Producatori> ProducersActive { get; set; }

        public string ErrorMessage { get; set; }

        public ObservableCollection<Producatori> GetAllProducers()
        {
            List<Producatori> producers = context.Producatoris.ToList();

            ObservableCollection<Producatori> result = new ObservableCollection<Producatori>();
            ProducersActive = new ObservableCollection<Producatori>();

            foreach (Producatori producer in producers)
            {
                result.Add(producer);

                if (producer.IsActive)
                {
                    ProducersActive.Add(producer);
                }
            }

            return result;
        }

        public void AddMethod(object obj)
        {
            Producatori producer = obj as Producatori;

            if (producer != null)
            {
                if (string.IsNullOrEmpty(producer.NumeProducator))
                {
                    ErrorMessage = "ProducerName is required";
                    return;
                }
                if (!checkUniqueProducer(producer.NumeProducator))
                {
                    return;
                }

                context.Producatoris.Add(producer);
                context.SaveChanges();
                producer.ProducatorID = context.Producatoris.Max(item => item.ProducatorID);
                Producers.Add(producer);
                ProducersActive.Add(producer);
                ErrorMessage = "";
            }
        }

        public void UpdateProducer(object obj)
        {
            Producatori producer = obj as Producatori;

            if (producer != null)
            {
                if (string.IsNullOrEmpty(producer.NumeProducator))
                {
                    ErrorMessage = "Name is required";
                    return;
                }

                if (!checkUniqueProducerName(producer))
                {
                    ErrorMessage = "Producer Exists";
                    return;
                }

                Producers[Producers.IndexOf(producer)] = producer;

                if (producer.IsActive)
                {
                    ProducersActive.Add(producer);
                }
                else
                {
                    ProducersActive.Remove(producer);
                }

                context.UpdateProducer(producer.ProducatorID, producer.NumeProducator, producer.TaraOrigine, producer.IsActive);
                context.SaveChanges();
                ErrorMessage = "";
            }
        }

        public void DeleteProducer(object obj)
        {
            Producatori producer = obj as Producatori;

            if (producer != null)
            {
                if (string.IsNullOrEmpty(producer.NumeProducator))
                {
                    ErrorMessage = "Name is required";
                }
                else
                {
                    context.DeactivateProducer(producer.ProducatorID);
                    context.SaveChanges();
                    ProducersActive.Remove(producer);
                    Producers[Producers.IndexOf(producer)].IsActive = false;
                    ErrorMessage = "";
                }
            }
        }

        private bool checkUniqueProducerName(Producatori producerToCheck)
        {
            foreach (Producatori producer in Producers)
            {
                if (producer.NumeProducator == producerToCheck.NumeProducator && producer.ProducatorID != producerToCheck.ProducatorID)
                {
                    return false;
                }
            }

            return true;
        }

        private bool checkUniqueProducer(string producerName)
        {
            foreach (Producatori producer in Producers)
            {
                if (producer.NumeProducator == producerName)
                {
                    if (!producer.IsActive) //IN CASE THERE IS A PRODUCER NOT ACTIVE WE UPDATE IT TO ACTIVE INSTEAD OF ADDING IT AGAIN
                    {
                        ProducersActive.Add(producer);
                        Producers[Producers.IndexOf(producer)].IsActive = true;
                        context.UpdateProducer(producer.ProducatorID, producer.NumeProducator, producer.TaraOrigine, producer.IsActive);
                        context.SaveChanges();
                    }
                    else
                    {
                        ErrorMessage = "Producer already exists";
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
