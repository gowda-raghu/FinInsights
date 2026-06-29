import { Routes } from '@angular/router';
import { Login } from '../modules/login/login';
import { Home } from '../modules/home/home/home';
import { authGuard } from './auth.guard';
import { Mutualfunds } from '../modules/mutualfunds/mutualfunds';
import { News } from '../modules/news/news';
import { CompareMutualfunds } from '../modules/compare-mutualfunds/compare-mutualfunds';
import { StockSearch } from '../modules/stock-search/stock-search';
import { Help } from '../modules/help/help';


export const routes: Routes = [
    { path: '', component: Login },
    { path: 'home', component: Home, canActivate: [authGuard] },
    { path: 'mf', component: Mutualfunds, canActivate: [authGuard] },
    { path: 'news', component: News, canActivate: [authGuard] },
    { path: 'compareMF', component: CompareMutualfunds, canActivate: [authGuard] },
    { path: 'searchStock', component:StockSearch, canActivate : [authGuard]},
    {path: 'help', component:Help, canActivate : [authGuard]}
];
