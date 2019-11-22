import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component'
import { HomeComponent } from './home/home.component'
import { PointsComponent } from './points/points.component'
import { NetworkSizeComponent } from './network-size/network-size.component'
import { ProductsComponent } from './products/products.component'
import { InvestmentsComponent } from './investments/investments.component'
import { QaComponent } from './qa/qa.component'

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'ns', component: NetworkSizeComponent },
  { path: 'points', component: PointsComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'investments', component: InvestmentsComponent },
  { path: 'qa', component: QaComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
