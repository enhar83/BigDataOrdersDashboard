using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Microsoft.ML;

namespace Business_Layer.MachineLearning.Concrete
{
    public class MessageSentimentTrainer
    {
        private readonly MLContext _mlContext;

        public MessageSentimentTrainer()
        {
            _mlContext = new MLContext(seed: 1);
        }

        public void TrainAndSaveModel(IEnumerable<MessageModelInputDto> trainingData, string modelPath)
        {
            IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(MessageModelInputDto.SentimentLabel)) //mapvaluetokey: sözel olan positive, negative etiketlerini bilgisayarın anlayacağı sayılara (0,1,2) çevirir
                .Append(_mlContext.Transforms.Text.FeaturizeText("Features", nameof(MessageModelInputDto.MessageText))) //featurizetext: stringi, matematiksel birer vektöre (sayı dizisine) çevirir. çünkü algoritmalar kelimeleri değil, sayıları hesaplar.
                .Append(_mlContext.MulticlassClassification.Trainers.OneVersusAll(
                    _mlContext.BinaryClassification.Trainers.FastTree())) //FastTree algoritması kullanılarak çoklu sınıflandıröa yapılır. yani modelin mesajı en olası kategoriye yerleştirmesini sağlıyorsun. 
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel")); //tahmin edilen sayıyı tekrar bizim anlayabileceğimiz positive kelimesine geri döndürür.

            var model = pipeline.Fit(dataView);

            _mlContext.Model.Save(model, dataView.Schema, modelPath); //.zip olarak kaydedilir, artık bu dosya mesaj okumayı bilen bir beyin gibidir.
        }
    }
}
