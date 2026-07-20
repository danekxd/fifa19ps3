--
-- PostgreSQL database dump
--

\restrict PhtBJr2991PgAnRNemEBMlkV4zGQeWAHuzacWf1dS1840Zxyd5Ye48puo3CuiCa

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: fcc12_stadium; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fcc12_stadium (
    carddbid integer,
    capacity integer,
    biodescription character varying,
    weightrare integer,
    cardassetid integer,
    description character varying,
    assetid integer,
    stadiumid integer,
    value integer,
    arenatype integer,
    name character varying,
    category integer,
    header character varying,
    zcat integer
);


ALTER TABLE public.fcc12_stadium OWNER TO postgres;

--
-- Data for Name: fcc12_stadium; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fcc12_stadium (carddbid, capacity, biodescription, weightrare, cardassetid, description, assetid, stadiumid, value, arenatype, name, category, header, zcat) FROM stdin;
6200000	40	StadiumDetailDesc	0	0	StadiumDesc_1	0	0	80	0	EA SPORTS ™ Arena1	4	Stadium	0
6200001	70	StadiumDetailDesc	0	0	StadiumDesc_2	1	0	80	0	EA SPORTS ™ Arena2	4	Stadium	1
6200002	90	StadiumDetailDesc	10	0	StadiumDesc_3	2	0	80	0	EA SPORTS ™ Arena3	4	Stadium	1
6200003	40	StadiumDetailDesc	0	1	StadiumDesc_4	3	0	80	1	EA SPORTS ™ Arena4	4	Stadium	2
6200004	70	StadiumDetailDesc	0	1	StadiumDesc_5	4	0	80	1	EA SPORTS ™ Arena5	4	Stadium	3
6200005	90	StadiumDetailDesc	10	1	StadiumDesc_6	5	0	80	1	EA SPORTS ™ Arena6	4	Stadium	3
6200006	100	StadiumDetailDesc	5	2	StadiumDesc_7	100	0	80	0	EA SPORTS ™ Arena7	4	Stadium	4
\.


--
-- PostgreSQL database dump complete
--

\unrestrict PhtBJr2991PgAnRNemEBMlkV4zGQeWAHuzacWf1dS1840Zxyd5Ye48puo3CuiCa

