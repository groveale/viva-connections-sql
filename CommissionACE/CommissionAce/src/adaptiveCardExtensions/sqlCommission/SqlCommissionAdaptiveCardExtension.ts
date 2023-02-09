import { IPropertyPaneConfiguration } from '@microsoft/sp-property-pane';
import { BaseAdaptiveCardExtension } from '@microsoft/sp-adaptive-card-extension-base';
import { CardView } from './cardView/CardView';
import { QuickView } from './quickView/QuickView';
import { SqlCommissionPropertyPane } from './SqlCommissionPropertyPane';
import { AadHttpClient } from '@microsoft/sp-http';
import { LeaderboardItem } from './models/LeaderboardItem';

export interface ISqlCommissionAdaptiveCardExtensionProps {
  title: string;
}

export interface ISqlCommissionAdaptiveCardExtensionState {
  daily: Number;
  weekly: Number;
  leaderboard: LeaderboardItem[]
}

const CARD_VIEW_REGISTRY_ID: string = 'SqlCommission_CARD_VIEW';
export const QUICK_VIEW_REGISTRY_ID: string = 'SqlCommission_QUICK_VIEW';

export default class SqlCommissionAdaptiveCardExtension extends BaseAdaptiveCardExtension<
  ISqlCommissionAdaptiveCardExtensionProps,
  ISqlCommissionAdaptiveCardExtensionState
> {
  private _deferredPropertyPane: SqlCommissionPropertyPane | undefined;

  public onInit(): Promise<void> {
    this.state = {
      daily: 0,
      weekly: 0,
      leaderboard: []
    };

    this.cardNavigator.register(CARD_VIEW_REGISTRY_ID, () => new CardView());
    this.quickViewNavigator.register(QUICK_VIEW_REGISTRY_ID, () => new QuickView());

    let userEmail = this.context.pageContext.user.email

    return this._fetchDataFromSQLAPI(userEmail);
  }

  protected loadPropertyPaneResources(): Promise<void> {
    return import(
      /* webpackChunkName: 'SqlCommission-property-pane'*/
      './SqlCommissionPropertyPane'
    )
      .then(
        (component) => {
          this._deferredPropertyPane = new component.SqlCommissionPropertyPane();
        }
      );
  }

  protected renderCard(): string | undefined {
    return CARD_VIEW_REGISTRY_ID;
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return this._deferredPropertyPane?.getPropertyPaneConfiguration();
  }

  private _fetchDataFromSQLAPI(userEmail: string): Promise<void> {
    return this.context.aadHttpClientFactory
      .getClient('91c459da-e9aa-41ae-b070-3d70592de2a2')
      .then(client => client.get(`https://ag-viva-connections-sql.azurewebsites.net/api/getcommission?userEmail=${userEmail}`, AadHttpClient.configurations.v1))
      .then(response => response.json())
      .then(commission => {
        this.setState({
          daily: commission.daily,
          weekly: commission.weekly,
          leaderboard: commission.leaderboard
        });
      });
  }
}
