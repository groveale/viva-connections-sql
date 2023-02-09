import { ISPFxAdaptiveCard, BaseAdaptiveCardView } from '@microsoft/sp-adaptive-card-extension-base';
import * as strings from 'SqlCommissionAdaptiveCardExtensionStrings';
import { LeaderboardItem } from '../models/LeaderboardItem';
import { ISqlCommissionAdaptiveCardExtensionProps, ISqlCommissionAdaptiveCardExtensionState } from '../SqlCommissionAdaptiveCardExtension';

export interface IQuickViewData {
  subTitle: string;
  title: string;
  leaderboard: LeaderboardItem[]
}

export class QuickView extends BaseAdaptiveCardView<
  ISqlCommissionAdaptiveCardExtensionProps,
  ISqlCommissionAdaptiveCardExtensionState,
  IQuickViewData
> {
  public get data(): IQuickViewData {
    return {
      subTitle: strings.SubTitle,
      title: strings.Title,
      leaderboard: this.state.leaderboard
    };
  }

  public get template(): ISPFxAdaptiveCard {
    return require('./template/LeaderboardView.json');
  }
}