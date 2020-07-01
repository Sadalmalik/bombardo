using System;

namespace Bombardo.V1
{
    class ListSugarContext
    {
        public static void Setup(Context context)
        {
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAAR, Caar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CADR, Cadr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDAR, Cdar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDDR, Cddr, 1);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAAAR, Caaar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAADR, Caadr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CADAR, Cadar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CADDR, Caddr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDAAR, Cdaar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDADR, Cdadr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDDAR, Cddar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDDDR, Cdddr, 1);

            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAAAAR, Caaaar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAAADR, Caaadr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAADAR, Caadar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CAADDR, Caaddr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CADAAR, Cadaar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CADADR, Cadadr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CADDAR, Caddar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CADDDR, Cadddr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDAAAR, Cdaaar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDAADR, Cdaadr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDADAR, Cdadar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDADDR, Cdaddr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDDAAR, Cddaar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDDADR, Cddadr, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDDDAR, Cdddar, 1);
            BombardoLangClass.SetProcedure(context, AllNames.LISP_CDDDDR, Cddddr, 1);
        }

        public static Atom Caar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom; }
        public static Atom Cadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom; }
        public static Atom Cdar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next; }
        public static Atom Cddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next; }

        public static Atom Caaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.atom; }
        public static Atom Caadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.atom; }
        public static Atom Cadar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.atom; }
        public static Atom Caddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.atom; }
        public static Atom Cdaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.next; }
        public static Atom Cdadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.next; }
        public static Atom Cddar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.next; }
        public static Atom Cdddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.next; }

        public static Atom Caaaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.atom?.atom; }
        public static Atom Caaadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.atom?.atom; }
        public static Atom Caadar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.atom?.atom; }
        public static Atom Caaddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.atom?.atom; }
        public static Atom Cadaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.next?.atom; }
        public static Atom Cadadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.next?.atom; }
        public static Atom Caddar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.next?.atom; }
        public static Atom Cadddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.next?.atom; }
        public static Atom Cdaaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.atom?.next; }
        public static Atom Cdaadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.atom?.next; }
        public static Atom Cdadar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.atom?.next; }
        public static Atom Cdaddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.atom?.next; }
        public static Atom Cddaar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.atom?.next?.next; }
        public static Atom Cddadr(Atom args, Context context) { return ((Atom)args?.value)?.next?.atom?.next?.next; }
        public static Atom Cdddar(Atom args, Context context) { return ((Atom)args?.value)?.atom?.next?.next?.next; }
        public static Atom Cddddr(Atom args, Context context) { return ((Atom)args?.value)?.next?.next?.next?.next; }
    }
}
