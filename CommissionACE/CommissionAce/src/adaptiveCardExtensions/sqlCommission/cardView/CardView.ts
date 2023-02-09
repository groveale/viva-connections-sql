import {
  BasePrimaryTextCardView,
  IPrimaryTextCardParameters,
  IExternalLinkCardAction,
  IQuickViewCardAction,
  ICardButton
} from '@microsoft/sp-adaptive-card-extension-base';
import * as strings from 'SqlCommissionAdaptiveCardExtensionStrings';
import { ISqlCommissionAdaptiveCardExtensionProps, ISqlCommissionAdaptiveCardExtensionState, QUICK_VIEW_REGISTRY_ID } from '../SqlCommissionAdaptiveCardExtension';

export class CardView extends BasePrimaryTextCardView<ISqlCommissionAdaptiveCardExtensionProps, ISqlCommissionAdaptiveCardExtensionState> {
  public get cardButtons(): [ICardButton] | [ICardButton, ICardButton] | undefined {
    return [
      {
        title: strings.QuickViewButton,
        action: {
          type: 'QuickView',
          parameters: {
            view: QUICK_VIEW_REGISTRY_ID
          }
        }
      }
    ];
  }

  public get data(): IPrimaryTextCardParameters {
    return {
      primaryText: `£${this.state.daily} Today 💸`,
      description: `You have earned:\n£${this.state.weekly} so far this week`,
      title: this.properties.title
    };
  }

  public get onCardSelection(): IQuickViewCardAction | IExternalLinkCardAction | undefined {
    return {
      type: 'ExternalLink',
      parameters: {
        target: 'https://www.bing.com'
      }
    };
  }
}
