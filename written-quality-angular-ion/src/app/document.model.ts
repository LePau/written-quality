export class WrittenDocumentQuality {
    readonly weight: number = 0;
    readonly rank: number = 0;
    readonly name: string = "";
    readonly title: string = "";
    readonly description: string = "";

    constructor(quality?: any | WrittenDocumentQuality) {
        if (quality) {
            Object.assign(this, quality);
        }
    }

    public get rankPercent() {
        return Math.round(this.rank * 100);
    }

    public get rankColor() {
        if (this.rank < 0.33) return 'danger';
        else if (this.rank < 0.66) return 'warning';
        else if (this.rank <= 1) return 'success';
    }
}

export class WrittenDocumentAnalysis {
    readonly qualities: WrittenDocumentQuality[];
    readonly overall: WrittenDocumentQuality;
    readonly summary: string = "";

    constructor(analysis?: any | WrittenDocumentAnalysis) {
        if (analysis) {
            Object.assign(this, analysis);
            this.qualities = (analysis.qualities || []).map(q => new WrittenDocumentQuality(q));
            if (analysis.overall) this.overall = new WrittenDocumentQuality(analysis.overall);
        }
    }

}

export class WrittenDocument {
    readonly createdDate: string;
    readonly id: string = null;
    readonly markdown: string = "";
    readonly analysis: WrittenDocumentAnalysis = null;

    constructor(document?: WrittenDocument, analysis?: WrittenDocumentAnalysis) {
        if (document) {
            this.createdDate = document.createdDate;
            this.id = document.id;
            this.markdown = document.markdown;

            analysis = analysis || document.analysis;
            if (analysis) this.analysis = new WrittenDocumentAnalysis(analysis);
        } else {
            // for now, let the unique date server as the id
            this.createdDate = this.id = new Date().toISOString();
        }
    }

    public toJson() {
        return {
            id: this.id,
            createdDate: this.createdDate,
            markdown: this.markdown
        }
    }

}
