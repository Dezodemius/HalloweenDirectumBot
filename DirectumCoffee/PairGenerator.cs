﻿using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.util;

namespace DirectumCoffee
{
    public class PairGenerator
    {
        private StanfordCoreNLP pipeline;

        public PairGenerator()
        {
            Properties props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, sentiment");
            props.setProperty("ner.useSUTime", "false");
            this.pipeline = new StanfordCoreNLP(props);
        }

        public void GeneratePairs(Dictionary<long, string> profiles)
        {
            List<KeyValuePair<long, Annotation>> annotations = new List<KeyValuePair<long, Annotation>>();
            foreach (var profile in profiles)
            {
                Annotation annotation = new Annotation(profile.Value);
                pipeline.annotate(annotation);
                annotations.Add(new KeyValuePair<long, Annotation>(profile.Key, annotation));
            }
            foreach (var annotation in annotations) 
            {
                var user = BotDbContext.Instance.UserInfos
                    .Where(u => u.UserId == annotation.Key)
                    .FirstOrDefault();
                if (user == null)
                    return;
                user.KeyWords = ExtractKeywords(annotation.Value);
                BotDbContext.Instance.SaveChanges();
            }

            // HashSet<long> pairedUsers = new HashSet<long>();
            //
            // for (int i = 0; i < profiles.Count - 1; i++)
            // {
            //     var profile1 = profiles.ElementAt(i);
            //     var annotation1 = annotations[i];
            //
            //     if (pairedUsers.Contains(profile1.Key))
            //     {
            //         continue;
            //     }
            //
            //     long bestMatchUserId = 0;
            //     int maxCommonKeywords = 0;
            //     string[] commonInterests = null;
            //
            //     for (int j = i + 1; j < profiles.Count; j++)
            //     {
            //         var profile2 = profiles.ElementAt(j);
            //         var annotation2 = annotations[j];
            //
            //         if (pairedUsers.Contains(profile2.Key))
            //         {
            //             continue;
            //         }
            //
            //         var keywords1 = ExtractKeywords(annotation1);
            //         var keywords2 = ExtractKeywords(annotation2);
            //
            //         var commonKeywords = keywords1.Intersect(keywords2).ToList();
            //
            //         int commonCount = commonKeywords.Count;
            //
            //         if (commonCount > maxCommonKeywords)
            //         {
            //             maxCommonKeywords = commonCount;
            //             bestMatchUserId = profile2.Key;
            //             commonInterests = commonKeywords.ToArray();
            //         }
            //     }
            //
            //     if (bestMatchUserId != 0)
            //     {
            //         var pair = new CoffeePair
            //         {
            //             FirstUserId = profile1.Key,
            //             SecondUserId = bestMatchUserId,
            //             CommonInterests = commonInterests
            //         };
            //
            //         BotDbContext.Instance.CoffeePairs.Add(pair);
            //         pairedUsers.Add(profile1.Key);
            //         pairedUsers.Add(bestMatchUserId);
            //     }
            // }
            //
            // BotDbContext.Instance.SaveChanges();
        }

        private List<string> ExtractKeywords(Annotation annotation)
        {
            string punctuationAndSymbols = ",.!?:;…-—'\"“”‘’!?\t\n\r/\\@#$%&*+-=<>()[]{}";
            string[] insignificantWords = {
                "а", "и", "или", "но", "да", "также", "тоже",
                "как", "что", "чтобы", "если", "когда", "пока", "потому", "чем",
                "с", "от", "до", "в", "на", "у", "о", "из", "перед", "под", "за",
                "при", "без", "по", "над", "после", "про", "между", "для", "во",
                "со", "к", "об", "поэтому", "таким образом", "следовательно",
                "тем не менее", "однако", "всё равно", "всё же", "так и", "то есть",
                "например", "кстати", "возможно", "может быть", "бывает",
                "как бы", "впрочем", "впрочём", "хотя", "даже", "только", "лишь",
                "вот", "всего", "чтож", "ну", "тут", "там", "здесь", "туда", "сюда", "тогда",
                "потом", "ибо", "ещё", "всегда", "всюду", "просто", "несмотря на", "причём"
            };

            List<string> keywords = new List<string>();

            var sentences = annotation.get(new CoreAnnotations.SentencesAnnotation().getClass()) as ArrayList;
            if (sentences != null)
            {
                foreach (CoreMap sentence in sentences)
                {
                    var words = sentence.get(new CoreAnnotations.TokensAnnotation().getClass()) as ArrayList;
                    if (words != null)
                    {
                        foreach (CoreLabel word in words)
                        {
                            string lemma = word.getString(new CoreAnnotations.LemmaAnnotation().getClass());
                            if (!string.IsNullOrWhiteSpace(lemma) && 
                                !punctuationAndSymbols.Contains(lemma) &&
                                !insignificantWords.Contains(lemma))
                            {
                                keywords.Add(lemma.ToLower());
                            }
                        }
                    }
                }
            }

            return keywords;
        }
    }
}
