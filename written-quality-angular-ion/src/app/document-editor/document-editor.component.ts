import { Component, OnInit, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { defaultValueCtx, Editor, rootCtx, editorViewCtx, serializerCtx, parserCtx } from '@milkdown/core';
import { commonmark, doc } from '@milkdown/preset-commonmark';
import { nord } from '@milkdown/theme-nord';
import { history } from '@milkdown/plugin-history';
import { clipboard } from '@milkdown/plugin-clipboard';
import { slash } from '@milkdown/plugin-slash';
import { listener, listenerCtx } from '@milkdown/plugin-listener';
import { BehaviorSubject, Subscription } from 'rxjs';
import { debounceTime, tap, throttleTime } from 'rxjs/operators';
import { asyncScheduler } from 'rxjs';
import { Slice } from "prosemirror-model";
import { WrittenDocument } from '../document.model';
import { DocumentViewService } from '../document-view.service';


@Component({
  selector: 'app-document-editor',
  templateUrl: './document-editor.component.html',
  styleUrls: ['./document-editor.component.scss'],
})
export class DocumentEditorComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('milkEditor') milkEditor: ElementRef;
  public defaultValue = '';
  public editor: Editor;
  public document = new WrittenDocument();
  private markdownUpdated$: BehaviorSubject<any> = new BehaviorSubject(null);
  private markdownUpdatedSubscription: Subscription = null;
  private documentNavigateSubscription: Subscription;

  constructor(
    public documentViewService: DocumentViewService
  ) { }

  ngOnInit() {
    this.documentNavigateSubscription = this.documentViewService.listenForNavigates((document: WrittenDocument) => {
      this.setDocument(document);
    });
  }

  ngOnDestroy(): void {
    this.markdownUpdatedSubscription?.unsubscribe();
    this.documentNavigateSubscription?.unsubscribe();
  }

  async ngAfterViewInit() {
    this.markdownUpdatedSubscription = this.markdownUpdated$
      .pipe(
        throttleTime(200, asyncScheduler, { leading: false, trailing: true }),
      ).subscribe(() => {
        this.addMarkdownToDocument(this.getMarkdown());
        this.documentViewService.update(this.document);
      });

    await this.createEditor(this.defaultValue);

  }

  async createEditor(defaultValue: string) {


    this.editor = await Editor.make()
      .config((ctx) => {
        ctx.set(rootCtx, this.milkEditor.nativeElement);
        ctx.set(defaultValueCtx, defaultValue);
        ctx.get(listenerCtx).updated((ctx, doc, prevDoc) => {
          this.markdownUpdated$.next(null);
        });
      })
      .use(nord)
      .use(history)
      .use(clipboard)
      .use(slash)
      .use(commonmark)
      .use(listener)
      .create();


  }

  getMarkdown(): string {
    return this.editor?.action((ctx) => {
      const editorView = ctx.get(editorViewCtx);
      const serializer = ctx.get(serializerCtx);
      return serializer(editorView.state.doc);
    });
  }

  setMarkdown(markdown: string) {
    this.addMarkdownToDocument(markdown);
    this.editor?.action((ctx) => {

      const view = ctx.get(editorViewCtx);
      const parser = ctx.get(parserCtx);
      const doc = parser(markdown);
      if (!doc) return;
      const state = view.state;
      view.dispatch(
        state.tr.replace(
          0,
          state.doc.content.size,
          new Slice(doc.content, 0, 0)
        )
      );
    });
  }

  clear() {
    this.document = new WrittenDocument();
    this.setMarkdown(this.document.markdown);
  }

  getDocument(): WrittenDocument {
    return this.document;
  }

  setDocument(newDocument: WrittenDocument) {
    this.document = newDocument;
    this.setMarkdown(this.document.markdown);
  }

  addMarkdownToDocument(markdown: string) {
    this.document = new WrittenDocument(Object.assign({}, this.document, { markdown }));    
  }
}
