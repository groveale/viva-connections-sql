import { ISPFxAdaptiveCard, BaseAdaptiveCardView } from '@microsoft/sp-adaptive-card-extension-base';
import * as strings from 'SqlCommissionAdaptiveCardExtensionStrings';
import { ISqlCommissionAdaptiveCardExtensionProps, ISqlCommissionAdaptiveCardExtensionState } from '../SqlCommissionAdaptiveCardExtension';

export interface IQuickViewData {
  subTitle: string;
  title: string;
}

export class QuickView extends BaseAdaptiveCardView<
  ISqlCommissionAdaptiveCardExtensionProps,
  ISqlCommissionAdaptiveCardExtensionState,
  IQuickViewData
> {
  public get data(): IQuickViewData {
    return {
      subTitle: strings.SubTitle,
      title: strings.Title
    };
  }

  public get template(): ISPFxAdaptiveCard {
    return require('./template/QuickViewTemplate.json');
  }
}