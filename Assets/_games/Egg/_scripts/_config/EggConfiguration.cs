using Antura.Database;
using Antura.Teacher;
using System;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public enum EggVariation
    {
        LetterName = MiniGameCode.Egg_lettername,
        LetterPhoneme = MiniGameCode.Egg_letterphoneme,
        BuildWord = MiniGameCode.Egg_buildword,
        Image = MiniGameCode.Egg_image
    }

    public class EggConfiguration : AbstractGameConfiguration
    {
        public EggVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (EggVariation)code;
        }

        // Singleton Pattern
        static EggConfiguration instance;
        public static EggConfiguration Instance
        {
            get {
                if (instance == null) {
                    instance = new EggConfiguration();
                }
                return instance;
            }
        }

        private EggConfiguration()
        {
            // Default values
            Context = new MinigamesGameContext(MiniGameCode.Egg_letterphoneme, System.DateTime.Now.Ticks.ToString());
            Variation = EggVariation.LetterPhoneme;

            if (Variation == EggVariation.BuildWord) {
                Questions = new SampleEggSequenceQuestionProvider();
            } else {
                Questions = new SampleEggSingleQuestionProvider();
            }
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 6;
            int nWrong = 7;

            // Debug.LogWarning("SetupBuilder " + Variation.ToString());

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctSeverity = SelectionSeverity.AsManyAsPossible;

            switch (Variation) {
                case EggVariation.LetterName:
                    builder = new RandomLettersQuestionBuilder(nPacks, 1, nWrong, parameters: builderParams);
                    break;
                case EggVariation.Image:
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
                    break;
                case EggVariation.BuildWord:
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.letterFilters.includeSpecialCharacters = true;
                    builderParams.letterFilters.includeAccentedLetters = true;
                    builder = new LettersInWordQuestionBuilder(5, nWrong: nWrong, useAllCorrectLetters: true, parameters: builderParams, maximumWordLength:8, removeAccents:false);
                    break;
                case EggVariation.LetterPhoneme:
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, 1, nWrong, parameters: builderParams, letterAlterationFilters: LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters_OneForm, avoidWrongLettersWithSameSound: true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder;
        }

        public override MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            // example: a.minigameVoteSkewOffset = 1f;
            return rules;
        }

        #region Localization IDs
        public override bool AutoPlayIntro => false;
        #endregion

        public bool IsSingleVariation()
        {
            switch (Variation) {
                case EggVariation.LetterName:
                case EggVariation.LetterPhoneme:
                case EggVariation.Image:
                    return true;
                case EggVariation.BuildWord:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsSequence()
        {
            switch (Variation) {
                case EggVariation.LetterName:
                case EggVariation.LetterPhoneme:
                case EggVariation.Image:
                    return false;
                case EggVariation.BuildWord:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}