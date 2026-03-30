import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from './services/api.service';
import { InsightArticle } from './models/article.model';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [CommonModule],
    template: `
    <div class="container">
      <header>
        <h1>Smart Insights Hub</h1>
        <p>KI-generierte Marktbeobachtung</p>
      </header>

      <div class="article-grid">
        <div class="card" *ngFor="let article of articles">
          <div class="card-header">
            <span class="ai-badge">KI-Zusammenfassung</span>
            <h3>{{ article.originalTitle }}</h3>
          </div>
          <div class="card-body">
            <p>{{ article.aiSummary }}</p>
          </div>
          <div class="card-footer">
            <span class="tag" *ngFor="let tag of article.tags">#{{ tag }}</span>
          </div>
        </div>
      </div>
    </div>
  `,
    styles: [`
    .container { padding: 40px; font-family: 'Inter', sans-serif; background: #f9fafb; min-height: 100vh; }
    header { text-align: center; margin-bottom: 50px; }
    h1 { color: #111827; font-size: 2.5rem; margin: 0; }
    header p { color: #6b7280; margin-top: 10px; }
    
    .article-grid { 
      display: grid; 
      grid-template-columns: repeat(auto-fill, minmax(350px, 1fr)); 
      gap: 25px; 
    }

    .card { 
      background: white; border-radius: 12px; border: 1px solid #e5e7eb;
      box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1); transition: transform 0.2s;
      padding: 24px; display: flex; flex-direction: column;
    }
    .card:hover { transform: translateY(-5px); }

    .ai-badge { 
      background: #eff6ff; color: #2563eb; font-size: 11px; 
      font-weight: 600; padding: 4px 10px; border-radius: 6px; 
      display: inline-block;
    }
    
    h3 { margin: 16px 0 12px 0; color: #111827; font-size: 1.25rem; line-height: 1.4; }
    p { color: #4b5563; line-height: 1.6; font-size: 14px; margin: 0; }
    
    .card-footer { margin-top: auto; padding-top: 20px; display: flex; flex-wrap: wrap; gap: 8px; }
    .tag { font-size: 12px; color: #6366f1; background: #f5f3ff; padding: 4px 8px; border-radius: 4px; font-weight: 500; }
  `]
})
export class AppComponent implements OnInit {
    articles: InsightArticle[] = [];

    constructor(private apiService: ApiService) { }

    ngOnInit() {
        this.apiService.getArticles().subscribe({
            next: (data) => this.articles = data,
            error: (err) => console.error(err)
        });
    }
}